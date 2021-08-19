create view part3.vw_projects_in_programmes as
(
    select pp.programme_id, prog.title as programme_title, pp.project_id, proj.title as project_title, proj.[year], proj.student_id
    from part3.programme_projects as pp
    inner join part3.programmes as prog
    on pp.programme_id = prog.id
    inner join part3.projects as proj
    on pp.project_id = proj.id
)