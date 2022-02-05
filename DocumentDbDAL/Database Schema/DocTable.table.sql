USE [DocumentDb]
GO

/****** Object:  Table [dbo].[DocTable]    Script Date: 2/5/2022 8:51:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DocTable](
	[id] [uniqueidentifier] NOT NULL,
	[category] [nvarchar](50) NOT NULL,
	[JsonDoc] [nvarchar](max) NOT NULL,
	[Owner] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastChanged] [datetime] NOT NULL,
	[KeyValue] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DocTable] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[DocTable] ADD  CONSTRAINT [DF_DocTable_category]  DEFAULT ('General') FOR [category]
GO

ALTER TABLE [dbo].[DocTable] ADD  CONSTRAINT [DF_DocTable_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[DocTable] ADD  CONSTRAINT [DF_DocTable_DateLastChanged]  DEFAULT (getdate()) FOR [DateLastChanged]
 
/****** Object:  Index [IXDocTable_Category]    Script Date: 2/5/2022 9:04:53 AM ******/
CREATE NONCLUSTERED INDEX [IXDocTable_Category] ON [dbo].[DocTable]
(
	[category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
 
/****** Object:  Index [IXDocTable_keyvalue_unique]    Script Date: 2/5/2022 9:03:13 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IXDocTable_keyvalue_unique] ON [dbo].[DocTable]
(
	[category] ASC,
	[KeyValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
