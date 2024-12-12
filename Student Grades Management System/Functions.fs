module Student_Grades_Management_System.Functions

open System.IO
open System

let readFileTo2DArray filePath =
    // Check if the file exists
    if File.Exists(filePath) then
        // Read all lines from the file and filter out blank lines
        let lines = 
            File.ReadAllLines(filePath)
            |> Array.filter (fun line -> not (String.IsNullOrWhiteSpace(line)))
        
        // Convert each line into an array 
        let textArray = 
            lines 
            |> Array.map (fun line -> line.Split(','))  // Split each line by comma
        
        textArray  // Return the 2D array
    else
        // Return an empty array if the file doesn't exist
        [||]
let appendToFile filePath content =
    File.AppendAllText(filePath, "\n" + content)

let writeToFile filePath content =
    File.WriteAllLines(filePath, content)

let users = readFileTo2DArray "../../../Files/users.txt"
let mutable students=readFileTo2DArray "../../../Files/students.txt"
let refreshData () =
    students <- readFileTo2DArray "../../../Files/students.txt"
let Studentsdata () =
   refreshData()
   (students)
let login (username:string) (password:string) =
    match Array.tryFind (fun (user:string[]) -> user.[0] = username && user.[1] = password) users with
    | Some user -> (user.[2])
    | None -> "Invalid username or password."
let addStudent (id:string)(name:string)(grade1:string)(grade2:string)(grade3:string) =
    match Array.tryFind (fun (student:string[]) -> student.[0] = id) students with
    | Some _ -> "ID Is Allready Taken."
    | None ->
        let newStudentLine = sprintf "%s,%s,%s,%s,%s" id name grade1 grade2 grade3
        appendToFile "../../../Files/students.txt" newStudentLine
        "Student added successfully."
// Filter out the line that contains the student ID
let DeleteStudent (id: string) =
    // Read all lines from the file
    let lines = File.ReadAllLines("../../../Files/students.txt")

    let updatedLines = 
        lines
        |> Array.filter (fun line -> 
            let parts = line.Split(',')
            parts.Length > 0 && parts.[0] <> id
        )

    // Check if the student was found and removed
    if lines.Length = updatedLines.Length then
        "Student not found."
    else
        // Write the updated lines back to the file
        writeToFile "../../../Files/students.txt" updatedLines
        "Student removed successfully."

let StudentData (id:string) =
    let studentsData=Studentsdata()
    match Array.tryFind (fun (student:string[]) -> student.[0] = id) studentsData with
    | Some student -> (student)
    | None -> [|"Student Not Found."|]

// Function to update a student's information by ID
let UpdateStudent (id: string) (newName: string) (newGrade1: string) (newGrade2: string) (newGrade3: string) =

    // Read all lines from the file
    let lines = File.ReadAllLines("../../../Files/students.txt")

    // Create updated lines by replacing the student's data
    let updatedLines =
        lines
        |> Array.map (fun line ->
            let parts = line.Split(',')
            if parts.Length > 0 && parts.[0] = id then
                // Update the student data
                sprintf "%s,%s,%s,%s,%s" id newName newGrade1 newGrade2 newGrade3
            else
                // Keep the line unchanged
                line
        )

    // Write the updated lines back to the file
    File.WriteAllLines("../../../Files/students.txt", updatedLines)
    "Student updated successfully."
// Function to calculate averages
let calculateAverages (data: string[][]) =
    data
    |> Array.map (fun row ->
        // Extract the student ID and name
        let id = row.[0]
        let name = row.[1]

        // Parse the grades from columns 3, 4, and 5
        let grade1 = float row.[2]
        let grade2 = float row.[3]
        let grade3 = float row.[4]

        // Calculate the average
        let average = (grade1 + grade2 + grade3) / 3.0

        // Return a tuple of (ID, Name, Average)
        [|id; name;string average|]
    )
let HighestGrades (data: string[][]) =
    data
    |> Array.map (fun row ->
        
        // Extract last 3 columns and convert to float
        let grade1 = data |> Array.map (fun row -> float row.[2])
        let grade2 = data |> Array.map (fun row -> float row.[3])
        let grade3 = data |> Array.map (fun row -> float row.[4])

        
        [|Array.max grade1;Array.max grade2; Array.max grade3|]
    )
let LowestGrades (data: string[][]) =
    data
    |> Array.map (fun row ->
        
        // Extract last 3 columns and convert to float
        let grade1 = data |> Array.map (fun row -> float row.[2])
        let grade2 = data |> Array.map (fun row -> float row.[3])
        let grade3 = data |> Array.map (fun row -> float row.[4])

        
        [|Array.min grade1;Array.min grade2; Array.min grade3|]
    )
let Passfill (data: string[][]) =
    data
    |> Array.map (fun row ->
        
        // Extract last 3 columns and convert to float
        let passfill = data |> Array.map (fun row -> float row.[2]) |> Array.sum
        
        (passfill/float data.Length)
    )
