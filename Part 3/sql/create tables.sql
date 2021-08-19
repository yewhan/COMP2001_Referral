create schema part3
go

create table part3.students (
    id int not null identity(1, 1),
    first_name nvarchar(30) not null,
    last_name nvarchar(30) not null,

    constraint pk_student_id primary key (id)
);

create table part3.projects (
    id int not null identity(1, 1),
    title nvarchar(30) not null,
    description nvarchar(320) not null,
    year nvarchar(9) not null,
    student_id int not null,

    constraint pk_project_id primary key (id),
    constraint fk_project_student_id foreign key (student_id) references part3.students(id) on delete no action
);

create table part3.programmes (
    id int not null identity(1, 1),
    title nvarchar(30) not null,
    student_id int,

    constraint pk_programme_id primary key (id),
    constraint fk_programme_student_id foreign key (student_id) references part3.students(id) on delete no action
);

create table part3.projects_audit (
    id int not null identity(1, 1),
    project_id int not null,
    student_id int not null,
    title nvarchar(30) not null,
    description nvarchar(320) not null,
    year nvarchar(9) not null,
    audit_action_code nvarchar(1) not null,
    audit_date_time smalldatetime not null
        constraint df_audit_date_time default CURRENT_TIMESTAMP,

    constraint pk_project_audit_id primary key (id, project_id),
    constraint fk_audited_project_id foreign key (project_id) references part3.projects(id) on delete no action
)

create table part3.programme_projects (
    programme_id int not null,
    project_id int not null,

    constraint pk_programme_projects primary key (programme_id, project_id),
    constraint fk_programme_id foreign key (programme_id) REFERENCES part3.programmes(id) on delete no action,
    constraint fk_project_id foreign key (project_id) references part3.projects(id) on delete no action
);