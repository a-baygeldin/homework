(*
    Alexander Baygeldin (c) 2014
    Huffman
*)

module Huffman

type CodeTree = 
  | Fork of CodeTree * CodeTree * char list * int
  | Leaf of char * int

let weight (tree: CodeTree) : int =
    match tree with
    | CodeTree (_, _, _, x) -> x
    | Leaf (_, x) -> x

let chars (tree: CodeTree) : char list = 
    match tree with
    | CodeTree (_, _, x, _) -> x
    | Leaf (x, _) -> x::[]

let times (list: char list) : (char * int) list =
    let orderedList = List.sortBy (fun x -> x) list
    match orderedList with
    | hd::tl -> 
        let rec times' list buf amount acc =
            match list with
            | hd::tl when hd = buf -> times' tl buf (amount + 1) acc
            | hd::tl when hd <> buf-> times' tl hd 1 ((buf, amount)::acc)
            | [] -> (buf, amount)::acc
        times' tl hd 1 []
    | [] -> []

let makeOrderedList (list: (char*int) list) : CodeTree list =
    let ctlist = List.sortBy (fun (x, y) -> abs y) list
    let rec makeOrderedList' list acc =
        match list with
        | (x, y)::tl -> makeOrderedList' tl ((Leaf (x, y))::acc)
        | [] -> acc
    makeOrderedList' ctlist []


let singleton (list: CodeTree list) : bool =
    match list with
    | hd::[] -> true
    | _ -> false

let rec combine (list: CodeTree list) : CodeTree list =
    if singleton list then 
        match list with
        | a::b::tl -> 
            match a, b with
            | CodeTree (_), CodeTree (_) -> combine (CodeTree(a, b, (chars a) @ (chars b), weight a + weight b)::tl)
            | CodeTree (_), Leaf (_) -> combine (CodeTree(a, b, (chars a) @ (chars b), weight a + weight b)::tl)
            | Leaf (_), CodeTree (_) -> combine (CodeTree(a, b, (chars a) @ (chars b), weight a + weight b)::tl)
            | Leaf (_), Leaf (_) -> combine (CodeTree(a, b, (chars a) @ (chars b), weight a + weight b)::tl)
    else list

let createCodeTree (chars: string) : CodeTree = 
    let list = Seq.toList chars
    let ctlist = combine (makeOrderedList (times list))
    match ctlist with
    | hd::[] -> hd
    | _ -> failwith "Something wrong!"    

let myTree = createCodeTree "aabbcdbcbecbdebcaddde" in
    printfn "%A" myTree
