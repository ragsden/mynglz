CREATE PROCEDURE UserChatGet
	@FromUserId	INT,
	@ToUserId INT,
	@ChatId	BIGINT = 0,
	@NoOfChat INT = 0
AS
BEGIN
	DECLARE @High INT, 
			@Low INT,
			@IsHigherMessage BIT = 0

	IF @FromUserId > @ToUserId
		SELECT @High = @FromUserId, @Low = @ToUserId
	ELSE
		SELECT @High = @ToUserId, @Low = @FromUserId

	IF @NoOfChat = 0
		SELECT 
			ChatId
			,HigherUserID
			,LowerUserID
			,IsHigherMessage
			,UserMessage
			,CreatedDtTm 
		FROM UserChat (NOLOCK)
		WHERE 
			HigherUserID = @High
			AND LowerUserID = @Low
			AND ChatId > @ChatId
		ORDER BY CreatedDtTm DESC
	ELSE
		SELECT TOP (@NoOfChat)
			ChatId
			,HigherUserID
			,LowerUserID
			,IsHigherMessage
			,UserMessage
			,CreatedDtTm 
		FROM UserChat (NOLOCK)
		WHERE 
			HigherUserID = @High
			AND LowerUserID = @Low
			AND ChatId > @ChatId
		ORDER BY CreatedDtTm DESC

END
