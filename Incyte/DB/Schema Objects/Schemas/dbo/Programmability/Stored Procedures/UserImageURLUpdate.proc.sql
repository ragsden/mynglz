CREATE PROCEDURE UserImageURLUpdate
	@UserId	INT
	,@ImageURL VARCHAR(8000) = NULL
AS
BEGIN	
	DECLARE @OldImageURL VARCHAR(8000)
	SELECT @OldImageURL = ISNULL(PictureLocation,'') FROM UserInfo (NOLOCK) WHERE UserID = @UserId

	IF @OldImageURL != @ImageURL
	BEGIN
		UPDATE UserInfo SET PictureLocation = @ImageURL WHERE UserID = @UserId
		INSERT INTO UserImageHistory
		(UserID, PictureLocation)
		VALUES 
		(@UserId, @OldImageURL)
	END
END
