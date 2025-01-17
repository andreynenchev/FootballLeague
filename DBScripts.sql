USE [FootballLeague]
GO
/****** Object:  User [fluser]    Script Date: 26.6.2020 г. 04:48:06 ч. ******/
CREATE USER [fluser] FOR LOGIN [fluser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [fluser]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [fluser]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [fluser]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [fluser]
GO
ALTER ROLE [db_datareader] ADD MEMBER [fluser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [fluser]
GO
/****** Object:  Table [dbo].[Matches]    Script Date: 26.6.2020 г. 04:48:06 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matches](
	[MatchId] [int] IDENTITY(1,1) NOT NULL,
	[TeamId1] [int] NOT NULL,
	[TeamId2] [int] NOT NULL,
	[ResultTeam1] [int] NOT NULL,
	[ResultTeam2] [int] NOT NULL,
 CONSTRAINT [PK_Matches] PRIMARY KEY CLUSTERED 
(
	[MatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 26.6.2020 г. 04:48:06 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](100) NULL,
	[Result] [int] NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Teams] ADD  CONSTRAINT [DF_Teams_Result]  DEFAULT ((0)) FOR [Result]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_Teams] FOREIGN KEY([TeamId1])
REFERENCES [dbo].[Teams] ([TeamId])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_Teams]
GO
ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matches_Teams1] FOREIGN KEY([TeamId2])
REFERENCES [dbo].[Teams] ([TeamId])
GO
ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matches_Teams1]
GO
/****** Object:  StoredProcedure [dbo].[RefreshResults]    Script Date: 26.6.2020 г. 04:48:06 ч. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrey Nenchev
-- Description:	Resets all Teams Results to 0 and re-calculates them.
-- =============================================
CREATE PROCEDURE [dbo].[RefreshResults]
AS
BEGIN
	
	create table #temp(id int, res int)


	insert into #temp
		SELECT [TeamId1]
				,count(*)*3 as pointsToAdd1
		FROM [FootballLeague].[dbo].[Matches]
		where [ResultTeam1] > [ResultTeam2]
		group by [TeamId1]

	insert into #temp
		SELECT [TeamId2]
				,count(*)*3 as pointsToAdd2
		FROM [FootballLeague].[dbo].[Matches]
		where [ResultTeam1] < [ResultTeam2]
		group by [TeamId2]

	insert into #temp
		SELECT [TeamId1]
				,count(*) as pointsToAdd1
		FROM [FootballLeague].[dbo].[Matches]
		where [ResultTeam1] = [ResultTeam2]
		group by [TeamId1]

	insert into #temp
		SELECT [TeamId2]
				,count(*) as pointsToAdd2
		FROM [FootballLeague].[dbo].[Matches]
		where [ResultTeam1] = [ResultTeam2]
		group by [TeamId2]

	update Teams set Result = 0

	update Teams set Result = te.newres
	from (select id, sum(res) as newres
			from #temp
			group by id
			) te
	where Teams.TeamId = te.id



	drop table #temp
END

GO
