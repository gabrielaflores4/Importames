CREATE DATABASE IF NOT EXISTS importames;
USE importames;

CREATE TABLE clientes (
    id_cliente INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    telefono VARCHAR(8),
    correo VARCHAR(50),
    direccion VARCHAR(100),
    fecha_registro DATE,
    dui VARCHAR(20)
);

CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    username VARCHAR(50),
    password VARCHAR(50),
    rol VARCHAR(20),
    telefono VARCHAR(8),
    correo VARCHAR(200)
);

CREATE TABLE estados_veh (
    id_estado INT AUTO_INCREMENT PRIMARY KEY,
    nombre_estado VARCHAR(50)
);

CREATE TABLE vehiculos (
    id_vehiculo INT AUTO_INCREMENT PRIMARY KEY,
    id_cliente INT,
    id_estado INT,
    marca VARCHAR(50),
    modelo VARCHAR(50),
    anio INT,
    color VARCHAR(50),
    vin VARCHAR(50),
    costo DECIMAL(10,2),
    fecha_ingreso DATE,

    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente),
    FOREIGN KEY (id_estado) REFERENCES estados_veh(id_estado)
);

CREATE TABLE historial_estado (
    id_historial INT AUTO_INCREMENT PRIMARY KEY,
    id_vehiculo INT,
    id_estado INT,
    id_usuario INT,
    fecha_cambio DATETIME,
	
    FOREIGN KEY (id_vehiculo) REFERENCES vehiculos(id_vehiculo),
    FOREIGN KEY (id_estado) REFERENCES estados_veh(id_estado),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);