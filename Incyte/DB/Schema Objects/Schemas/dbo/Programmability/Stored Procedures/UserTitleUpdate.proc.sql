CREATE PROCEDURE UserTitleUpdate
	@UserId	INT
	,@Title NVARCHAR(140)
AS
BEGIN	
	UPDATE UserInfo SET InstantTitle = @Title WHERE UserID = @UserId
END