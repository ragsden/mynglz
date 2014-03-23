CREATE PROCEDURE VisibleProfilesByBusinessGet
	@UserId	INT
AS
BEGIN	
	DECLARE @BusinessID INT
			,@Preference SMALLINT
			
	SELECT	@BusinessID = 0
			,@Preference = 0 
	
	SELECT @BusinessID = BusinessId FROM UserCheckIn (NOLOCK) WHERE UserID = @UserId
	SELECT @Preference = Preference FROM UserInfo (NOLOCK) WHERE UserID = @UserId
	
	IF @Preference = 3
		SELECT u.* FROM UserInfo (NOLOCK) u
		JOIN UserCheckIn (NOLOCK) uc
		ON uc.UserID = u.UserID
		WHERE
			uc.BusinessId = @BusinessID
			AND u.OnlineStatusID > 0
			AND uc.UserID <> @UserId
			
	ELSE
		SELECT u.* FROM UserInfo (NOLOCK) u
		JOIN UserCheckIn (NOLOCK) uc
		ON uc.UserID = u.UserID
		WHERE
			uc.BusinessId = @BusinessID
			AND u.Gender = @Preference
			AND u.OnlineStatusID > 0
			AND uc.UserID <> @UserId
END
