namespace Student_Grades_Management_System

open System
open System.Windows.Forms
open System.Drawing
open System.Drawing.Printing
open Functions

module GlobalState =
    // Example of a global user role
    let mutable currentUserRole = ""

type AddStudent() as addStudent =
    inherit Form(Text="Add Student Form",Width = 600,Height = 400)
    let idlabel = new Label(Text = "ID", Location = System.Drawing.Point(30, 70), AutoSize = true)
    let namelabel = new Label(Text = "Name", Location = System.Drawing.Point(320, 70), AutoSize = true)
    let grade1label = new Label(Text = "Grade 1", Location = System.Drawing.Point(30, 130), AutoSize = true)
    let grade2label = new Label(Text = "Grade 2", Location = System.Drawing.Point(220, 130), AutoSize = true)
    let grade3label = new Label(Text = "Grade 3", Location = System.Drawing.Point(410, 130), AutoSize = true)
    let id = new TextBox(PlaceholderText="ID",Height=30,Width = 250,Location = Point(30,90))
    let name = new TextBox(PlaceholderText="Name",Height=30,Width = 250,Location = Point(320,90))
    let grade1 = new TextBox(PlaceholderText="Grade 1",Height=30,Width = 160,Location = Point(30,150))
    let grade2 = new TextBox(PlaceholderText="Grade 2",Height=30,Width = 160,Location = Point(220,150))
    let grade3 = new TextBox(PlaceholderText="Grade 3",Height=30,Width = 160,Location = Point(410,150))
    let add = new Button(Text = "Add Student", AutoSize = true,Visible = true,Location = Point(250, 240))
    let back = new Button(Text="back",AutoSize=true,Visible = true,Location=Point(500,20))
    //let adminform =new AdminForm()
    do
        addStudent.Controls.AddRange([|id;name;grade1;grade2;grade3;add;idlabel;namelabel;grade1label;grade2label;grade3label;back|])
        back.Click.Add(fun _ ->
            addStudent.Hide()
            //adminform.Show()
        )
        add.Click.Add(fun _ ->
            let result=Functions.addStudent id.Text name.Text grade1.Text grade2.Text grade3.Text
            MessageBox.Show($"{result}") |> ignore
        )

and DeleteStudentForm() as delete =
    inherit Form(Text="Delete Student Form",Width = 600,Height = 400)
    let IDlabel = new Label(Text = "Student ID", Location = System.Drawing.Point(150, 110), AutoSize = true)
    let ID = new TextBox(PlaceholderText = "Enter Student ID" ,Height=30,Width = 300,Location = Point(150,130)  )
    let DeleteButton = new Button(Text = "Remove Student",Height=30,Width = 150,Visible = true,Location = Point(225, 200))
    let back = new Button(Text="back",AutoSize=true,Visible = true,Location=Point(500,20))
    //let adminform =new AdminForm()
    do

        DeleteButton.Click.Add(fun _ ->
            let result=Functions.DeleteStudent ID.Text 
            MessageBox.Show($"{result}") |> ignore
        )
        back.Click.Add(fun _ ->
            delete.Hide()
            //adminform.Show()
        )
        // Add controls
        delete.Controls.AddRange([|IDlabel;ID;DeleteButton;back|])

and UpdateStudentForm() as update =
    inherit Form(Text="Update Student Form",Width = 600,Height = 400)
    let idlabel = new Label(Text = "ID", Location = System.Drawing.Point(175, 10), AutoSize = true)
    let namelabel = new Label(Text = "Name", Location = System.Drawing.Point(30, 100), AutoSize = true)
    let grade1label = new Label(Text = "Grade 1", Location = System.Drawing.Point(320, 100), AutoSize = true)
    let grade2label = new Label(Text = "Grade 2", Location = System.Drawing.Point(30, 160), AutoSize = true)
    let grade3label = new Label(Text = "Grade 3", Location = System.Drawing.Point(320, 160), AutoSize = true)
    let id = new TextBox(PlaceholderText="ID",Height=30,Width = 250,Location = Point(175,30))
    let name = new TextBox(PlaceholderText="Name",Height=30,Width = 250,Location = Point(30,120))
    let grade1 = new TextBox(PlaceholderText="Grade 1",Height=30,Width = 250,Location = Point(320,120))
    let grade2 = new TextBox(PlaceholderText="Grade 2",Height=30,Width = 250,Location = Point(30,180))
    let grade3 = new TextBox(PlaceholderText="Grade 3",Height=30,Width = 250,Location = Point(320,180))
    let UpdateButton = new Button(Text = "Update Student", AutoSize = true,Visible = true,Location = Point(250, 240))
    let SearchButton = new Button(Text = "Search", AutoSize = true,Visible = true,Location = Point(250, 70))
    let back = new Button(Text="back",AutoSize=true,Visible = true,Location=Point(500,20))
    //let adminform =new AdminForm()
    do
        update.Controls.AddRange([|id;idlabel;SearchButton;back|])
        back.Click.Add(fun _ ->
            update.Hide()
            //adminform.Show()
        )

        SearchButton.Click.Add(fun _ ->
            let result=Functions.StudentData id.Text
            if result.[0]=id.Text then
                begin
                    // Add controls to the form if they are not already added
                    if not (update.Controls.Contains(name)) then update.Controls.Add(name)
                    if not (update.Controls.Contains(grade1)) then update.Controls.Add(grade1)
                    if not (update.Controls.Contains(grade2)) then update.Controls.Add(grade2)
                    if not (update.Controls.Contains(grade3)) then update.Controls.Add(grade3)
                    if not (update.Controls.Contains(namelabel)) then update.Controls.Add(namelabel)
                    if not (update.Controls.Contains(grade1label)) then update.Controls.Add(grade1label)
                    if not (update.Controls.Contains(grade2label)) then update.Controls.Add(grade2label)
                    if not (update.Controls.Contains(grade3label)) then update.Controls.Add(grade3label)
                    if not (update.Controls.Contains(UpdateButton)) then update.Controls.Add(UpdateButton)
                    name.Text <- result.[1]
                    grade1.Text <- result.[2]
                    grade2.Text <- result.[3]
                    grade3.Text <- result.[4]
                end
            else
                // Add controls to the form if they are not already added
                if (update.Controls.Contains(name)) then update.Controls.Remove(name)
                if (update.Controls.Contains(grade1)) then update.Controls.Remove(grade1)
                if (update.Controls.Contains(grade2)) then update.Controls.Remove(grade2)
                if (update.Controls.Contains(grade3)) then update.Controls.Remove(grade3)
                if (update.Controls.Contains(namelabel)) then update.Controls.Remove(namelabel)
                if (update.Controls.Contains(grade1label)) then update.Controls.Remove(grade1label)
                if (update.Controls.Contains(grade2label)) then update.Controls.Remove(grade2label)
                if (update.Controls.Contains(grade3label)) then update.Controls.Remove(grade3label)
                if (update.Controls.Contains(UpdateButton)) then update.Controls.Remove(UpdateButton)
                MessageBox.Show($"{result.[0]}") |> ignore
        )
        UpdateButton.Click.Add(fun _ ->
            let result =Functions.UpdateStudent id.Text name.Text grade1.Text grade2.Text grade3.Text
            MessageBox.Show($"{result}") |> ignore

        )


