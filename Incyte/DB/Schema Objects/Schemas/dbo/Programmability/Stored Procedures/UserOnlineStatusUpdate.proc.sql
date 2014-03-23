CREATE PROCEDURE UserOnlineStatusUpdate
	@UserId	INT
	,@StatusId SMALLINT
AS
BEGIN	
	UPDATE UserInfo SET OnlineStatusID = @StatusId WHERE UserID = @UserId
END
