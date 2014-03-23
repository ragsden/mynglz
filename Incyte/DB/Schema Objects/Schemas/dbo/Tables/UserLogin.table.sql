CREATE TABLE [dbo].[UserLogin](
	[UserID] [int] NOT NULL IDENTITY(1,1),
	UserName	nvarchar(10) NULL,
	UserPassword nvarchar(20) null,
	[ExternalId] varchar (30) NULL,
	[SourceID] [smallint] NOT NULL,
	ExternalIdCheckSum varbinary(100) NOT NULL,
	LastLoginDateTime	DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] [varchar](20)  NULL,
	[Disabled] [bit] NOT NULL,
 CONSTRAINT [XPKUserLogin] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)
)
