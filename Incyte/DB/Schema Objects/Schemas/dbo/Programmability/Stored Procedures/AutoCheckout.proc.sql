CREATE PROCEDURE AutoCheckout
AS
BEGIN
	
	DECLARE @users TABLE (userid INT, isprocessed bit)
	DECLARE @count INT
			,@userid INT
	
	INSERT INTO @users
	SELECT
		UserID
		,0
	FROM UserCheckIn (NOLOCK)
	WHERE ISNULL(BusinessId,0) <> 0 
	AND DATEDIFF(HOUR,UpdatedDtTm,GETUTCDATE()) > 6 
	
	SELECT @count = COUNT(1) FROM @users WHERE isprocessed = 0
	WHILE @count > 0
	BEGIN
		
		SELECT TOP 1 @userid = userid from @users WHERE isprocessed = 0
		
		EXEC CheckOut @UserId = @userid , @BusinessID = 0

		UPDATE @users SET isprocessed = 1 WHERE userid = @userid
		
		SELECT @count = 0
		SELECT @count = COUNT(1) FROM @users WHERE isprocessed = 0
		
	END

END	

