USE [DbMiF]
GO
SELECT sng.SongName, 
	   alb.AlbumName, 
       alb.AlbumYear,
	   (select au.AuthorSurname from Authors au where au.AuthorID = sng.TextAuthorID) textAuthor,
	   (select au.AuthorSurname from Authors au where au.AuthorID = sng.MusicAuthorID) musicAuthor
  FROM Songs sng, Albums alb
 where sng.AlbumID = alb.AlbumID