// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
//let hs = new GLib.
//[<EntryPoint>]
//let main argv = 
//    Application.Init()
//    printfn "%A" argv
//
//
//    let app = new AppProgram()
//    let gxml = new Glade.XML(null, "gui.glade", "window1", null)
//    gxml.Autoconnect(app)
//
//    app.Button1.Label <- "hello"
//    app.Button1.Clicked.Add(fun(o) -> printfn "Hello dude")
//
//    Application.Run()
//    0 // return an integer exit code
// Create Window
// Show it to me
// return an integer exit code
module Program

open System
open Gtk
open Glade

type AppProgram() = 
    
    [<Glade.Widget>]
    let mutable button1 = new Button()
    
    member this.Button1 = button1

let listFiles dir = List.ofSeq( IO.Directory.EnumerateFiles(dir, "*.*", IO.SearchOption.AllDirectories))


let filterTree (model: TreeModel) iter =
    let artistName = model.GetValue(iter, 0)
    if artistName <> null then 
        artistName.ToString() <> "Dude" 
    else    
        false
        


let createTreeView() = 
    let tree = new TreeView()
    
    let artistColumn = new TreeViewColumn() 
    let artistRenderer = new CellRendererText()
    artistColumn.Title <- "Artists"
    artistColumn.PackStart(artistRenderer, true)
    artistColumn.AddAttribute(artistRenderer, "text", 0)

    let songColumn = new TreeViewColumn()
    let songRenderer = new CellRendererText()
    songColumn.Title <- "Songs"
    songColumn.PackStart(songRenderer, true)
    songColumn.AddAttribute(songRenderer, "text", 1)

    tree.AppendColumn(artistColumn) |> ignore
    tree.AppendColumn(songColumn) |> ignore
    tree

[<EntryPoint>]
let main argv = 
    Application.Init()
    printfn "%A" argv
    
    // Create main window
    let myWin = new Window("My First GTK# Application!")
    myWin.SetSizeRequest(500, 200)
    myWin.DeleteEvent.Add(fun _ -> Application.Quit())
    

    // Create entry
    let entry = new Entry()
    //entry.Changed
    let label = new Label("Thing Search:")


    // Put them in a Box, yay
    let filterBox = new HBox()
    filterBox.PackStart(label, false, false, 5u)
    filterBox.PackStart(entry, true , true , 5u)
  

    // Create and configure Tree
    let tree = createTreeView()
    let model = new ListStore(typeof<string>, typeof<string>)
    model.AppendValues([|"Dude"; "Cart"|]) |> ignore
    
 


    let box = new VBox()
    box.PackStart(filterBox, false, false, 5u)
    box.PackStart(tree     , true , true , 5u)
    
    myWin.Add(box)
    myWin.ShowAll()

       // Create filter for model
   
    let st = Diagnostics.Stopwatch.StartNew()

    model.AppendValues([|"Pannkaka"; "Ärtsoppa"|]) |> ignore
    //ignore <| List.map (fun(s) -> printfn "%s" s) (listFiles "C:\\Source" ) 
    (listFiles "c:\\source\\" )
        |> List.map (fun(s) -> model.AppendValues("file" ,s)) 
        |> List.sumBy (fun(e) -> 1)
        |> (printfn "%d")

    st.Stop()
    printfn "Took: %dms" st.ElapsedMilliseconds

    let filter = new TreeModelFilter(model, null)
    filter.VisibleFunc <- new TreeModelFilterVisibleFunc(filterTree)
    tree.Model <- filter
    
    Application.Run()
    0
