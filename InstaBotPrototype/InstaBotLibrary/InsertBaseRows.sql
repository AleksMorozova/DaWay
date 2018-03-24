INSERT INTO Users (Login, Password, Name, Surname, Birth)
VALUES ('user1','user1password','UserName','UserSurname', '1997-11-16');

INSERT INTO Bounds (UserId, TelegramAccount, InstagramToken)
VALUES (1,'TELEGA','INSTA_ACCESS_TOKEN');

INSERT INTO Filters (BoundId, Filter)
VALUES (1, 'Filter1');

INSERT INTO Filters (BoundId, Filter)
VALUES (1, 'Filter2');