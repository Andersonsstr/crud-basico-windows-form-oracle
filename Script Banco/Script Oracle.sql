drop table maquina;
drop sequence maquina_sequence;

CREATE TABLE maquina
( 
  id number null,
  hostname varchar2(100) not null,
  ip varchar2(100) not null,
  modelo varchar2(100) not null,
  setor varchar2(100) not null,
  foto blob null,
  data_cadastro date not null,
  
  CONSTRAINT pk_maquina PRIMARY KEY (id)
);

CREATE SEQUENCE maquina_sequence;

CREATE OR REPLACE TRIGGER maquina_on_insert
 BEFORE INSERT ON maquina
 FOR EACH ROW
BEGIN
 SELECT maquina_sequence.nextval
 INTO :new.id
 FROM dual;
END;

insert into maquina values ('', 'IEPAFGI28001', '192.168.0.2', 'DELL 780', 'IEP Logística', '', sysdate);
commit;

select * from maquina;
