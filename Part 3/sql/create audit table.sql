create table part3.projects_audit (
    id int not null identity(1, 1),
    project_id int not null,
    student_id int not null,
    title nvarchar(30) not null,
    description nvarchar(320) not null,
    year nvarchar(9) not null,
    audit_action_code tinyint not null,
    audit_date_time smalldatetime not null
        constraint df_audit_date_time default CURRENT_TIMESTAMP,

    constraint pk_project_audit_id primary key (id, project_id),
    constraint fk_audited_project_id foreign key (project_id) references part3.projects(id) on delete no action
)