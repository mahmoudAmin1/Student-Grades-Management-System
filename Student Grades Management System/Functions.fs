module Student_Grades_Management_System.Functions

open System.IO

let readFileTo2DArray filePath =
    // Check if the file exists
    if File.Exists(filePath) then
        // Read all lines from the file
        let lines = File.ReadAllLines(filePath)
        
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
let students=readFileTo2DArray "../../../Files/students.txt"

let Studentsdata () =
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
