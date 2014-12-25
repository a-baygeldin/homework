(*
    A. Baygeldin (c) 2014
    Hexic (NOT WORKING)
*)

module Hexic

open System

type Cell = int*int
type Triple = Cell*Cell*Cell
type Colours = Empty=0 | Red=1 | Blue=2 | Green=3 | Yellow=4 | Orange=5 | Azure=6

let size = 5
let random = new System.Random()
let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd
let (=.=) (x:Colours) (y:Colours) = x.Equals(y)

let bootstrap = 
    (fun _ j -> 
        match j with
        | Odd -> enum<Colours>(random.Next(1,4))
        | Even -> enum<Colours>(random.Next(4,7)))
    |> Array2D.init size size

let neighbours (map:Colours[,]) i j = 
    [(i-1,j); (i,j-1); (i,j+1); (i+1,j); (i+1,j-1); (i+1,j+1)]
    |> List.filter (fun x -> fst x >= 0 && snd x >= 0 && fst x < size && snd x < size)

let BFS (map:Colours[,]) (triple:Triple) =
    let root = match triple with | (x, y, z) -> [x; y; z]
    let colour = map.[fst root.Head, snd root.Head]
    let rec BFS' visited front = 
        let allNeighbours = List.concat [ for x in front do yield neighbours map (fst x) (snd x) ]
        let newFront = List.filter (fun x -> map.[(fst x),(snd x)] =.= colour) allNeighbours
        if newFront <> [] then BFS' (visited @ front) newFront else (visited @ front)
    BFS' root root
        
let countPoints (map:Colours[,]) (triple:Triple) = (BFS map triple).Length    

let turnCW (map:Colours[,]) (x:Cell) (y:Cell) (z:Cell) =
    let xc, yc, zc = (map.[fst x, snd x], map.[fst y, snd y], map.[fst z, snd z])
    map.[fst x, snd x] <- zc
    map.[fst y, snd y] <- xc
    map.[fst z, snd z] <- yc

let turnRightTriple map i j =
    match j with
    | _ when j = size - 1 -> ()
    | Odd -> turnCW map (i,j) (i+1,j+1) (i+1,j)
    | Even -> turnCW map (i,j) (i,j+1) (i+1,j)

let turnLeftTriple map i j =
    match j with
    | _ when j = 0 -> ()
    | Odd -> turnCW map (i,j) (i+1,j) (i+1,j-1)
    | Even -> turnCW map (i,j) (i+1,j) (i,j-1)

let findTriples (map:Colours[,]) =
    let result = ref []
    let checkout (x:Cell) (y:Cell) (z:Cell) =
        if (map.[fst x, snd x] =.= map.[fst y, snd y] && map.[fst y, snd y] =.= map.[fst z, snd z]) then
            result := (x, y, z)::!result
    for i in 0 .. size - 2 do
        for j in 0 .. size - 1 do
            match j with
            | _ when j = 0 -> checkout (i,j) (i,j+1) (i+1,j)
            | Odd when j = size - 1 -> checkout (i,j) (i+1,j) (i,j-1)
            | Even when j = size - 1 -> checkout (i,j) (i+1,j) (i+1,j-1)
            | Odd -> 
                checkout (i,j) (i+1,j) (i+1,j-1)
                checkout (i,j) (i+1,j+1) (i+1,j-1)
            | Even -> 
                checkout (i,j) (i,j+1) (i+1,j)
                checkout (i,j) (i+1,j) (i,j-1)
    !result
        
let bestTriple (map:Colours[,]) (list:Triple list) =
    let rec bestTriple' list triple acc =
        match list with
        | hd::tl ->
            let count = countPoints map hd
            if count > acc then bestTriple' tl hd count
            else  bestTriple' tl triple acc
        | [] -> triple
    bestTriple' list.Tail list.Head (countPoints map list.Head)

let drop (map:Colours[,]) = 
    for i in 0 .. size - 1 do
        for j in 0 .. size - 1 do
            if map.[i,j] = Colours.Empty then  map.[i,j] <- enum<Colours>(random.Next(1,7))
    map

let refactor (map:Colours[,]) = 
    let triple = (findTriples map).Head
    let toRemove = BFS map triple
    let deleteCell (cell:Cell) =
        for i in fst cell .. size - 2 do
            map.[i, snd cell] <- map.[i+1, snd cell]
        map.[size-1, snd cell] <- Colours.Empty //WRONG! All cells are shifting.
    for cell in toRemove do deleteCell cell
    drop map

let play map =
    let rec play' map score =
        let buf = ref (Array2D.copy map)
        let max = ref 0
        let proceed () = 
            let triples = findTriples map
            let points = 
                match triples with
                | hd::tl -> countPoints map (bestTriple map triples)
                | [] -> 0
            if points > !max then 
                buf := (Array2D.copy map)
                max := points
        let apply turnFun map i j =
            for k in 0 .. 2 do
                turnFun map i j
                proceed ()
        for i in 0 .. size - 2 do
            for j in 0 .. size - 1 do
                apply turnLeftTriple map i j
                apply turnRightTriple map i j
        if !max = 0 then score else play' (refactor !buf) (score + !max) 
    play' map 0

printf "%A" (play bootstrap)
Console.ReadLine() |> ignore