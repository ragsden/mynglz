CREATE PROCEDURE UserChatAdd
	@FromUserId	INT,
	@ToUserId INT,
	@Message NVARCHAR (120)
AS
BEGIN
	DECLARE @High INT, 
			@Low INT,
			@IsHigherMessage BIT = 0

	IF @FromUserId > @ToUserId
		SELECT @High = @FromUserId, @Low = @ToUserId
	ELSE
		SELECT @High = @ToUserId, @Low = @FromUserId

		IF @High = @FromUserId
			SELECT @IsHigherMessage = 1
	

	INSERT INTO UserChat
	(
		HigherUserID,
		LowerUserID,
		IsHigherMessage,
		UserMessage
	)
	SELECT
		@High,
		@Low,
		@IsHigherMessage,
		@Message

	SELECT @@IDENTITY AS ChatId

END
