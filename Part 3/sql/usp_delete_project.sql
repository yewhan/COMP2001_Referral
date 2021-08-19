create procedure part3.usp_delete_project (@id as int)

as
begin 
    begin transaction 
        declare @error nvarchar(max);
        begin try 

            if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'projects')
            begin 
                if exists (select 1 from part3.projects where id = @id)
                begin
                    delete from part3.projects where id = @id;
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