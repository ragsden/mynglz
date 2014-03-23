CREATE PROCEDURE UserSelectionGet
	@UserId	INT,
	@TypeId	SMALLINT = 0 /* 0=ALL, 1=LIKES, 2=DISLIKES, 3=HITS (order by touser likes) 4=MISSES (order by touser likes) 5=My choices 6=others choices*/
AS
BEGIN	
	
	IF @TypeId = 1
		SELECT ToUserId AS ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 2
		SELECT ToUserId ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
			AND StatusId = 0
	ELSE IF @TypeId = 3
		SELECT FromUserID ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID IN 
				(SELECT ToUserId FROM UserSelection (NOLOCK)
				WHERE FromUserID = @UserId AND StatusId = 1)
			AND ToUserId = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 4
		SELECT FromUserID ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID IN 
				(SELECT ToUserId FROM UserSelection (NOLOCK)
				WHERE FromUserID = @UserId AND StatusId = 0)
			AND ToUserId = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 5
		SELECT ToUserId AS ToUserId, StatusId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
	ELSE IF @TypeId = 6
		SELECT FromUserID AS ToUserId, StatusId FROM UserSelection (NOLOCK)
		WHERE 
			ToUserId = @UserId
			
END

