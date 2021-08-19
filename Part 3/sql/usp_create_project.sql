create procedure part3.usp_create_project (@student_id as int, @title as nvarchar(30), @description as nvarchar(320), 
    @year as nvarchar(9), @response_message nvarchar(256) output)
as
begin 
    begin transaction
        declare @error nvarchar(max);
        begin try

            if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'students')
                and exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'projects')
                begin --check tables exist

                    if not exists (select 1 from part3.projects where student_id = @student_id and year = @year)
                    begin 

                        if exists (select 1 from part3.students where id = @student_id) --check student exists
                        begin

                            insert into part3.projects(student_id, title, description, year)
                                values (@student_id, @title, @description, @year);

                            set @response_message = '201';
                        end

                        else
                        begin
                            set @response_message = '404';
                        end
                    end
                    else
                    begin
                        set @response_message = '208';
                    end
                end
                
            else 
            begin 
                set @response_message = '404';
                set @error = 'Critical error - missing tables';
                throw 2706, @error, 1;
            end

            if @@trancount > 0 commit;
        end try

        begin catch
            if @@trancount > 0 rollback transaction;
        end catch
    end