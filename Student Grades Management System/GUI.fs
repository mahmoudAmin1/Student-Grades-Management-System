namespace Student_Grades_Management_System

open System
open System.Windows.Forms
open System.Drawing
open System.Drawing.Printing
open Functions

module GlobalState =
    // Example of a global user role
    let mutable currentUserRole = ""

    
type MainForm() as main =
    inherit Form(Text="Login Form",Width = 600,Height = 400)
    let mutable auth =""
    let username = new TextBox(PlaceholderText = "Enter your Username" ,Height=30,Width = 300,Location = Point(150,90)  )
    let password = new TextBox(PlaceholderText = "Enter your Password",Height=30,Width = 300,Location = Point(150,150),PasswordChar = '*'  )
    let loginButton = new Button(Text = "Login",Height=30,Width=60,Visible = true,Location = Point(270, 240))
    let AdminForm = new AdminForm()
    let UserForm =new UserForm()
    // Create a label
    let userlabel = new Label(Text = "Username", Location = System.Drawing.Point(150, 70), AutoSize = true)
    let passlabel = new Label(Text = "Password", Location = System.Drawing.Point(150, 130), AutoSize = true)
    do

        loginButton.Click.Add(fun _ ->
            let result=Functions.login username.Text password.Text
            if result ="admin" then 
                begin
                    GlobalState.currentUserRole <- "admin"
                    main.Hide()
                    AdminForm.Show()
                end
             else if result="user" then
                GlobalState.currentUserRole <- "user"
                main.Hide()
                UserForm.Show()
             else
                MessageBox.Show($"{result}") |> ignore
        )

        // Add controls
        main.Controls.AddRange([|loginButton;username;password;userlabel;passlabel|])


and AdminForm() as admin =
    inherit Form(Text = "Admin Main Form",Width = 600,Height = 400  )

    let logout = new Button(Text="Log Out",AutoSize=true,Visible = true,Location=Point(500,20))
    let Add = new Button(Text = "Add Student",Visible = true,Height =30,Width = 150,Location =Point(225, 50))
    let UpdateButton = new Button(Text = "Update Student",Visible = true,Height =30,Width = 150,Location =Point(225, 100))
    let Remove = new Button(Text = "Delete Student",Visible = true,Height =30,Width = 150,Location =Point(225,150))
    let ViewStudents = new Button(Text = "View Students",Visible = true,Height =30,Width = 150,Location =Point(225, 200))
    let ViewStatistics = new Button(Text = "View Statistics",Visible = true,Height =30,Width = 150,Location =Point(225, 250))
    
    do 
        admin.Controls.AddRange([|Add;UpdateButton;Remove;ViewStudents;ViewStatistics;logout|])
        // Button click event handler
        logout.Click.Add(fun _ ->
                    let mainform = new MainForm()
                    GlobalState.currentUserRole <- ""
                    admin.Hide()
                    mainform.Show()
        )
        ViewStudents.Click.Add(fun _ ->
                    let StudentsForm = new ViewStudentsForm()
                    admin.Hide()
                    StudentsForm.Show()
        )
        ViewStatistics.Click.Add(fun _ ->
                    let StatisticsForm= new StatisticsForm()
                    admin.Hide()
                    StatisticsForm.Show()
        )
        Add.Click.Add(fun _ ->
                    let AddStudentForm =new AddStudent()
                    admin.Hide()
                    AddStudentForm.Show()
        ) 
        Remove.Click.Add(fun _ ->
                    let DeleteForm =new DeleteStudentForm()
                    admin.Hide()
                    DeleteForm.Show()
        )
        UpdateButton.Click.Add(fun _ ->
                    let UpdateForm =new UpdateStudentForm()
                    admin.Hide()
                    UpdateForm.Show()
        )
and UserForm() as user =
    inherit Form(Text = "User Main Form",Width = 600,Height = 400  )

    let logout = new Button(Text="Log Out",AutoSize=true,Visible = true,Location=Point(500,20))
    let ViewStudents = new Button(Text = "View Students",Visible = true,Height =30,Width = 150,Location =Point(225, 100))
    let ViewStatistics = new Button(Text = "View Statistics",Visible = true,Height =30,Width = 150,Location =Point(225, 200))
    
    do 
        user.Controls.AddRange([|ViewStudents;ViewStatistics;logout|])
        // Button click event handler
        logout.Click.Add(fun _ ->
                    let mainform = new MainForm()
                    GlobalState.currentUserRole <- ""
                    user.Hide()
                    mainform.Show()
        )
        ViewStudents.Click.Add(fun _ ->
                    let StudentsForm = new ViewStudentsForm()
                    user.Hide()
                    StudentsForm.Show()
        )
        ViewStatistics.Click.Add(fun _ ->
                    let StatisticsForm= new StatisticsForm()
                    user.Hide()
                    StatisticsForm.Show()
        )

and ViewStudentsForm() as student =
    inherit Form(Text="View Students Form",Width=600,Height=400)
    let studentsData= Functions.Studentsdata()
    let dataGrid = new DataGridView(Visible = true,AllowUserToDeleteRows = false,AllowUserToAddRows = false,ReadOnly = true,Width=600,Location=Point(0,40),Height=360 )
    let back = new Button(Text="back",AutoSize=true,Visible = true,Location=Point(500,10))
    let adminform =new AdminForm()
    let userForm =new UserForm()
        // Load data into the DataGridView
    let loadData () =
        dataGrid.ColumnCount <- 5
        dataGrid.Columns.[0].Name <- "ID"
        dataGrid.Columns.[1].Name <- "Name"
        dataGrid.Columns.[2].Name <- "Grade 1"
        dataGrid.Columns.[3].Name <- "Grade 2"
        dataGrid.Columns.[4].Name <- "Grade 3"
        
        dataGrid.Rows.Clear()
        for student in studentsData do
             dataGrid.Rows.Add(student.[0], student.[1], student.[2], student.[3], student.[4]) |> ignore

    do
        student.Controls.AddRange([|dataGrid;back|])
        // Load data when the form is created
        student.Load.Add(fun _ -> loadData())
        dataGrid.Refresh()
        back.Click.Add(fun _ ->
            if GlobalState.currentUserRole = "admin" then
                begin
                    student.Hide()
                    adminform.Show()
                end
            else 
                student.Hide()
                userForm.Show()
        )
        // Optional: Expose a public method to reload data
    member student.RefreshData() =
        loadData()
        MessageBox.Show($"Student data: {studentsData}") |> ignore 

and StatisticsForm() as statistics =
    inherit Form(Text = "Students Statistics",Width=600,Height=400)
    let students =Functions.Studentsdata()
    let averageData =Functions.calculateAverages(students)
    let Highestgrade =Functions.HighestGrades(students)
    let Lowestgrade = Functions.LowestGrades(students)
    let passfill =Functions.Passfill(averageData)
    let dataGrid = new DataGridView(Dock = DockStyle.Left,Width = 375,ReadOnly = true,AllowUserToAddRows = false,AllowUserToDeleteRows = false,Visible = true)
    let back = new Button(Text="back",AutoSize=true,Visible = true,Location=Point(500,20))
    let adminform =new AdminForm()
    let userForm =new UserForm()
        // Load data into the DataGridView
    let loadData () =
        dataGrid.ColumnCount <- 3
        dataGrid.Columns.[0].Name <- "ID"
        dataGrid.Columns.[1].Name <- "Name"
        dataGrid.Columns.[2].Name <- "Average"
        
        dataGrid.Rows.Clear()
        for average in averageData do
            let formattedAverage = 
                match System.Double.TryParse(average.[2]) with
                | (true, value) -> sprintf "%.2f" value
                | _ -> average.[2]  // If parsing fails, keep the original string
            dataGrid.Rows.Add(average.[0],average.[1],formattedAverage)  |> ignore
    // Create a label control
    let HighestGrades = new Label(Visible = true,AutoSize = true,Location = new Point(400,50),Text = sprintf "The Highest Grades:\n\nSubject 1: %.2f\nSubject 2: %.2f\nSubject 3: %.2f" Highestgrade.[0].[0] Highestgrade.[0].[1] Highestgrade.[0].[2])
    let LowestGrades = new Label(Visible = true,AutoSize = true,Location = new Point(400,140),Text = sprintf "\nThe Lowest Grades:\n\nSubject 1: %.2f\nSubject 2: %.2f\nSubject 3: %.2f" Lowestgrade.[0].[0] Lowestgrade.[0].[1] Lowestgrade.[0].[2])
    let Passfill = new Label(Visible = true,AutoSize = true,Location = new Point(400,270),Text = sprintf "The Pass/Fill Rate:\n\nAll Subjects : %.2f"  passfill.[0])
    do
        statistics.Controls.AddRange([|dataGrid;HighestGrades;LowestGrades;Passfill;back|])
        // Load data when the form is created
        statistics.Load.Add(fun _ -> loadData())
        dataGrid.Refresh()

        back.Click.Add(fun _ ->
            if GlobalState.currentUserRole = "admin" then
                begin
                    statistics.Hide()
                    adminform.Show()
                end
            else 
                statistics.Hide()
                userForm.Show()
        )
        // Optional: Expose a public method to reload data
    member statistics.RefreshData() =
        loadData()

and AddStudent() as addStudent =
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
    let adminform =new AdminForm()
    do
        addStudent.Controls.AddRange([|id;name;grade1;grade2;grade3;add;idlabel;namelabel;grade1label;grade2label;grade3label;back|])
        back.Click.Add(fun _ ->
            addStudent.Hide()
            adminform.Show()
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
    let adminform =new AdminForm()
    do

        DeleteButton.Click.Add(fun _ ->
            let result=Functions.DeleteStudent ID.Text 
            MessageBox.Show($"{result}") |> ignore
        )
        back.Click.Add(fun _ ->
            delete.Hide()
            adminform.Show()
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
    let adminform =new AdminForm()
    do
        update.Controls.AddRange([|id;idlabel;SearchButton;back|])
        back.Click.Add(fun _ ->
            update.Hide()
            adminform.Show()
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


