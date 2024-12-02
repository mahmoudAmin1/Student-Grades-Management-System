module Student_Grades_Management_System.MainForm

open System
open System.Windows.Forms

type MainForm() as this =
    inherit Form()

    let buttonShow = new Button(Text = "Show Students", Dock = DockStyle.Bottom)
    let dataGrid = new DataGridView(Dock = DockStyle.Fill)

    do
        // Form properties
        this.Text <- "Student Grades Management System"
        this.Width <- 600
        this.Height <- 400

        // Add controls
        this.Controls.Add(dataGrid)
        this.Controls.Add(buttonShow)

        // Button event handler
        buttonShow.Click.Add(fun _ -> this.LoadStudents())

    member private this.LoadStudents() =
        // Sample data
        let students = [|
            [| "1"; "Alice"; "90, 85, 78" |]
            [| "2"; "Bob"; "75, 88, 91" |]
        |]

        // Set up the DataGridView columns
        dataGrid.ColumnCount <- 3
        dataGrid.Columns.[0].Name <- "ID"
        dataGrid.Columns.[1].Name <- "Name"
        dataGrid.Columns.[2].Name <- "Grades"

        // Clear existing rows and add new data
        dataGrid.Rows.Clear()
        for student in students do
            dataGrid.Rows.Add(student) |> ignore
