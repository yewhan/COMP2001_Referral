create procedure part3.usp_update_project (@project_id as int, @title as nvarchar(30), @description as nvarchar(320),
    @year as nvarchar(9), @student_id as int) --passed in project id instead of student id due to students having the ability to upload multiple projects
                          --if student id was used, multiple projects could potentially be updated by mistake
as
begin
    begin transaction
        declare @error nvarchar(max);
        begin try

            if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'projects')
            begin

                if @year is not null
                begin

                    if exists (select 1 from part3.projects where id = @project_id)
                    begin
                        update part3.projects set title = @title where id = @project_id and student_id = @student_id and year = @year 
                        and @title is not null;

                        update part3.projects set description = @description where id = @project_id and student_id = @student_id 
                            and year = @year and @description is not null;

                        --update part3.projects set year = @year where id = @project_id and student_id = @student_id and @year is not null;
                    end
                end
            end

            else
            begin
                set @error = 'Critical error - missing tables';
                throw 2706, @error, 1;
            end 

            if @@trancount > 0 commit;
        end try

        begin catch
            if @@trancount > 0 rollback transaction;
        end catch
    end