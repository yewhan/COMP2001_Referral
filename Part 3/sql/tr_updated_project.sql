create trigger tr_updated_project on part3.projects
after update 
as 
begin
    if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'projects_audit')
    begin 
        insert into part3.projects_audit(project_id, student_id, title, description, year, audit_action_code)
            select id, student_id, title, description, year, 'U'
            from deleted;
    end
end 