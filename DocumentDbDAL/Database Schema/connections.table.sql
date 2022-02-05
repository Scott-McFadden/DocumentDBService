USE [DocumentDb]
GO

/****** Object:  Table [dbo].[Connections]    Script Date: 2/5/2022 8:47:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Connections](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](250) NOT NULL,
	[description] [nvarchar](500) NULL,
	[engineType] [nvarchar](10) NOT NULL,
	[dbName] [nvarchar](50) NULL,
	[connectionString] [nvarchar](500) NOT NULL,
	[collectionName] [nvarchar](250) NOT NULL,
	[uniqueKeys] [nvarchar](250) NULL,
	[tags] [nvarchar](250) NULL,
	[UserId] [nvarchar](250) NULL,
	[Password] [nvarchar](250) NULL,
	[AuthModel] [nvarchar](250) NULL,
 CONSTRAINT [PK_Connections] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Connections] ADD  CONSTRAINT [DF_Connections_AuthModel]  DEFAULT (N'none') FOR [AuthModel]
GO
 
/****** Object:  Index [IXConnectionsName]    Script Date: 2/5/2022 8:59:45 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IXConnectionsName] ON [dbo].[Connections]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO 



