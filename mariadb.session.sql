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