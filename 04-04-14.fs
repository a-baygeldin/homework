module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int


// code tree

let createCodeTree (chars: char list) : CodeTree = 
    failwith "Not implemented"

// decode

type Bit = int

let decode (tree: CodeTree)  (bits: Bit list) : char list = 
  failwith "Not implemented"

// encode

let rec unite lf ls =
    match lf with
    | [] -> ls
    | hd::tl -> hd::(unite tl ls)

let rec bitlist tree acc res = 
    match tree with
    | Fork (x, y , _ , _) -> unite (bitlist x (0::acc) res) (bitlist y (1::acc) res)
    | Leaf (x, _ ) -> (x, acc)::res

let rec encode text blist =
    let bitcode a list =
        match list with
        | (x,y)::tl when x = a -> y
        | [] -> []   
    match text with
    | hd::tl -> unite (bitcode hd blist) (encode tl blist) 
    | [] -> []

let blist = bitlist (Fork (Fork (Fork (Leaf('d', 1), Leaf ('c', 2), ['c';'d'], 3), Leaf ('b', 2), ['b';'c';'d'], 5), Leaf ('a', 3), ['a';'b';'c';'d'], 8)) [0] []
    in printf "encode = %A" (encode ['a';'a';'b';'c';'a';'b';'c';'d'] blist)