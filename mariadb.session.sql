-- create table test(
--     id int not null primary key AUTO_INCREMENT,
--     name varchar(100) not null
-- );
-- insert into test(name)
-- values("test1"),("test2");
-- select * from test;
GRANT ALL PRIVILEGES ON *.* TO 'root' IDENTIFIED BY 'root' WITH GRANT OPTION;
FLUSH PRIVILEGES;
-- show global variables like 'log_error';
-- ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'root';
SELECT user, host, plugin FROM mysql.user;
create user 'backend'@'localhost' IDENTIFIED by 'pwd'
GRANT ALL PRIVILEGES ON *.* TO 'backend'@'localhost' WITH GRANT OPTION;
FLUSH PRIVILEGES;

drop table *;
drop table test;
SELECT * FROM aspnetusers, aspnetusers;
select * from aspnetusers

SELECT CONCAT('DROP TABLE IF EXISTS ', table_name, ';')
FROM information_schema.tables
WHERE table_schema = 'szakdol';
CREATE database szakdol;
drop database szakdol;
use szakdol;
select * from competitionuser;

drop database test;
DROP TABLE IF EXISTS aspnetroleclaims;
DROP TABLE IF EXISTS aspnetroles;
DROP TABLE IF EXISTS aspnetuserclaims;
DROP TABLE IF EXISTS aspnetuserlogins;
DROP TABLE IF EXISTS aspnetuserroles;
DROP TABLE IF EXISTS aspnetusers;
DROP TABLE IF EXISTS aspnetusertokens;
DROP TABLE IF EXISTS test;
DROP TABLE IF EXISTS __efmigrationshistory;
