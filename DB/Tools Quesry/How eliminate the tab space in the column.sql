SELECT REPLACE(Email, char(9), '') From AspNetUsers
update AspNetUsers set Email= REPLACE(Email, char(9), '')
update AspNetUsers set Email = LTRIM(RTRIM(Email)) 