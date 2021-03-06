CREATE DATABASE [music_store]
GO
USE [music_store]
GO
/****** Object:  Table [dbo].[AlbumGenre]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumGenre](
	[GenreID] [int] NOT NULL,
	[AlbumID] [int] NOT NULL,
 CONSTRAINT [PK_AlbumGenre] PRIMARY KEY CLUSTERED 
(
	[GenreID] ASC,
	[AlbumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Albums]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albums](
	[AlbumID] [int] IDENTITY(1,1) NOT NULL,
	[ArtistID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Price] [money] NULL,
	[ProducerID] [int] NULL,
	[AlbumName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[AlbumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Artists]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[ArtistID] [int] IDENTITY(1,1) NOT NULL,
	[ArtistName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
(
	[ArtistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Genre]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[GenreID] [int] IDENTITY(1,1) NOT NULL,
	[GenreName] [nchar](100) NOT NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[GenreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Producers]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producers](
	[ProducerID] [int] IDENTITY(1,1) NOT NULL,
	[ProducerName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Producers] PRIMARY KEY CLUSTERED 
(
	[ProducerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Songs]    Script Date: 31.7.2013 г. 14:18:49 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[SongID] [int] IDENTITY(1,1) NOT NULL,
	[AlbumID] [int] NOT NULL,
	[SongName] [nvarchar](160) NOT NULL,
	[Duration] [time](2) NOT NULL,
 CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AlbumGenre]  WITH CHECK ADD  CONSTRAINT [FK_AlbumGenre_Albums] FOREIGN KEY([AlbumID])
REFERENCES [dbo].[Albums] ([AlbumID])
GO
ALTER TABLE [dbo].[AlbumGenre] CHECK CONSTRAINT [FK_AlbumGenre_Albums]
GO
ALTER TABLE [dbo].[AlbumGenre]  WITH CHECK ADD  CONSTRAINT [FK_AlbumGenre_Genre] FOREIGN KEY([GenreID])
REFERENCES [dbo].[Genre] ([GenreID])
GO
ALTER TABLE [dbo].[AlbumGenre] CHECK CONSTRAINT [FK_AlbumGenre_Genre]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Artists] FOREIGN KEY([ArtistID])
REFERENCES [dbo].[Artists] ([ArtistID])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Artists]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Producers] FOREIGN KEY([ProducerID])
REFERENCES [dbo].[Producers] ([ProducerID])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Producers]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Albums] FOREIGN KEY([AlbumID])
REFERENCES [dbo].[Albums] ([AlbumID])
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_Albums]
GO
