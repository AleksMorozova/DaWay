INSERT INTO Users (Login, Password, Name, Surname, Birth)
VALUES ('user1','user1password','UserName','UserSurname', '1997-11-16');

INSERT INTO Bounds (UserId, TelegramAccount, InstagramAccount, InstagramPassword)
VALUES (1,'TELEGA','INSTA_ACC','INSTA_PASS');

INSERT INTO Filters (BoundId, Filter)
VALUES (1, 'Filter1');

INSERT INTO Filters (BoundId, Filter)
VALUES (1, 'Filter2');