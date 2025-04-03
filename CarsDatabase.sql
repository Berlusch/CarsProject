create table car_owners(
id int not null primary key identity(1,1),
first_name varchar(20) not null,
last_name varchar(20) not null,
date_of_birth date not null
);

create table car_engine_types(
id int not null primary key identity(1,1),
type varchar(20) not null,
abrv varchar(10) not null
);

create table car_makes(
id int not null primary key identity(1,1),
name varchar(20) not null,
abrv varchar(10) not null
);
-------------------------
create table car_models(
id int not null primary key identity(1,1),
name varchar(20) not null,
abrv varchar(10) not null,
car_make int not null references car_makes(id),
car_engine_type int not null references car_engine_types(id)
);

create table car_registrations(
id int not null primary key identity(1,1),
registration_number int not null,
car_owner int not null references car_owners(id),
car_model int not null references car_models(id)
);

ALTER TABLE car_registrations
ALTER COLUMN registration_number VARCHAR(20) not null;

insert into car_owners(first_name, last_name, date_of_birth) VALUES
('John', 'Smith', '1985-06-15'),
('Emma', 'Johnson', '1992-09-23'),
('Michael', 'Williams', '1978-12-05'),
('Sophia', 'Brown', '1989-03-11'),
('Daniel', 'Davis', '1995-07-30');

insert into car_engine_types(type, abrv) VALUES
('Petrol', 'PET'),
('Diesel', 'DSL'),
('Electric', 'ELEC'),
('Hybrid', 'HYB'),
('Hydrogen', 'H2');

insert into car_makes(name, abrv) VALUES
('Toyota', 'TOY'),
('Ford', 'FRD'),
('Honda', 'HON'),
('BMW', 'BMW'),
('Mercedes-Benz', 'MB');

insert into car_models(name, abrv, car_make, car_engine_type)VALUES
('Corolla', 'COR', 1, 1),  -- Toyota, Petrol
('Camry', 'CAM', 1, 2),    -- Toyota, Diesel
('Civic', 'CIV', 3, 1),    -- Honda, Petrol
('Accord', 'ACC', 3, 4),   -- Honda, Hybrid
('Mustang', 'MST', 2, 1);  -- Ford, Petrol

insert into car_registrations(registration_number, car_owner, car_model)VALUES
('ABC1234', 1, 1),  -- John Smith, Toyota Corolla
('XYZ5678', 2, 3),  -- Emma Johnson, Honda Civic
('LMN4321', 3, 2),  -- Michael Williams, Toyota Camry
('DEF8765', 4, 4),  -- Sophia Brown, Honda Accord
('PQR1357', 5, 5);  -- Daniel Davis, Ford Mustang