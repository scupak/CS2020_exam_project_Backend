# -CS2020_exam_project_Backend

Admin User:
{
    "firstName": "Admin",
    "lastName": "Admin",
    "doctorEmailAddress": "Admin@gmail.com",
    "phoneNumber": "11111111",
    "isAdmin": true,
    "password": "1234"
}
"doctorEmailAddress": "Admin@gmail.com",
    "firstName": "Admin",
    "lastName": "Admin",
    "phoneNumber": "11111111",
    "isAdmin": false,
    "passwordHash": "pOIjfNgj4bUYclITcQ/1fyH35IZSRSRQFtGIjzfziJA9/Rr9pGX8TE2WDTnDGiuVjQGjj2hBcq/9XBplu+wRnA==",
    "passwordSalt": "ydP/0dewbdXPNY+OnFcIO+mTwuOVJ3mK2Pfj7EKCr9Pz8LdUio5QFLqzwxlancAM5CuTeIpFvnGMuKoTCXk9fp3fSu3pWGGOXICOeXZsyIpGcZRwdWLNes62fdtbw9vMNaSY/CC4KtQ76j3JX14Dn0c4j64swPt2X2AAHgKLWBc=",
    "appointments": null
    
 select DoctorEmailAddress, convert(varchar(max),[dbo].[Doctors].[PasswordSalt], 1) from [dbo].[Doctors]


Query to insert the first admin into the databse. The password is 1234. The username is 4Admin@gmail.com
Insert Into [dbo].[Doctors] (doctorEmailAddress, firstName,lastName , phoneNumber,isAdmin,passwordHash,passwordSalt  ) VALUES ('4Admin@gmail.com','Admin', 'Admin', '11111111', 1 ,convert(varbinary(max), 0xA4E2237CD823E1B518725213710FF57F21F7E4865245245016D1888F37F388903DFD1AFDA465FC4C4D960D39C31A2B958D01A38F684172AFFD5C1A65BBEC119C), convert(varbinary(max), 0xC9D3FFD1D7B06DD5CF358F8E9C57083BE993C2E39527798AD8F7E3EC4282AFD3F3F0B7548A8E5014BAB3C3195A9DC00CE42B93788A45BE718CB8AA1309793D7E9DDF4AEDE958618E5C808E79766CC88A467194707562CD7ACEB67DDB5BC3DBCC35A498FC20B82AD43BEA3DC95F5E039F47388FAE2CC0FB765F60001E028B5817) )









