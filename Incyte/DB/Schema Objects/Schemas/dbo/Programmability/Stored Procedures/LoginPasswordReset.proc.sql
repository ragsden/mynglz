CREATE PROCEDURE LoginPasswordReset
	@UserId INT = 0,
	@UserName	nvarchar(10) = NULL,
	@UserPassword nvarchar(20) = NULL
AS
BEGIN
	
	IF @UserId = 0
	BEGIN
		UPDATE UserLogin 
		SET UserPassword = @UserPassword
		WHERE externalidCheckSum = HASHBYTES('MD5',@UserName) AND UserName = @UserName
	END
	ELSE
	BEGIN
		UPDATE UserLogin 
		SET UserPassword = @UserPassword
		WHERE UserID = @UserId
	END

END
