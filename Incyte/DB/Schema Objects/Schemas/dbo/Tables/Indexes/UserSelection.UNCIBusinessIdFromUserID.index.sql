CREATE UNIQUE NONCLUSTERED INDEX UNCIBusinessIdFromUserID ON UserSelection(
FromUserID,
ToUserId
)