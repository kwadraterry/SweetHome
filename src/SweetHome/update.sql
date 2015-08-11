
    create table shelters (
        Id int4 not null,
       Name varchar(255) not null,
       Address varchar(160),
       Phone varchar(255),
       primary key (Id)
    )
    create table animals (
        Id int4 not null,
       Name varchar(255) not null,
       AnimalType int4 not null,
       BirthDay date,
       Color int4,
       Created timestamp not null,
       ImagesSerialized text,
       Info varchar(500),
       IsForFlat boolean,
       IsForHome boolean,
       IsHappy boolean,
       IsHealthy boolean,
       PlaceType int4 not null,
       Size int4,
       Toilet boolean,
       Gender int4 not null,
       Shelter_id int4,
       primary key (Id)
    )
    alter table animals 
        add constraint FKCCEC31F71CD43FF0 
        foreign key (Shelter_id) 
        references shelters
    alter table shelters 
        add column Info text
    create table animals (
        Id int4 not null,
       Name varchar(255) not null,
       AnimalType int4 not null,
       BirthDay date,
       Color int4,
       Created timestamp not null,
       ImagesSerialized text,
       Info text,
       IsForFlat boolean,
       IsForHome boolean,
       IsHappy boolean,
       IsHealthy boolean,
       PlaceType int4 not null,
       Size int4,
       Toilet boolean,
       Gender int4 not null,
       Shelter_id int4,
       primary key (Id)
    )
    alter table animals 
        add constraint FKCCEC31F71CD43FF0 
        foreign key (Shelter_id) 
        references shelters
    alter table shelters 
        add column VKGroup varchar(500)
    alter table shelters 
        add column Image varchar(500)