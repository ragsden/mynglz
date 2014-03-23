CREATE PROCEDURE UserInfoGet
	@UserId	INT
AS
BEGIN	
	SELECT * from UserInfo (NOLOCK) WHERE  UserID  = @UserId
END
