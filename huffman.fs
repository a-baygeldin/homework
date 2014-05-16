(*
A. Baygeldin (c) 2014
Huffman
*)

module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int

type Bit = int

let unite lf ls = 
    let rec unite' lf ls f =
        match lf with
        | [] -> f ls
        | hd::tl -> unite' tl ls (fun x -> f (hd::x))
    unite' lf ls (fun x -> x)

let reverseList list = List.fold (fun acc elem -> elem::acc) [] list

let decode (tree: CodeTree)  (bits: Bit list) : char list = 
    let rec decode' (tree: CodeTree) (subtree: CodeTree)  (bits: Bit list) (acc: char list) : char list = 
        match bits with
        | hd::tl -> 
           match subtree with
            | Fork (l, r, _, _) -> if hd = 1 then (decode' tree r tl acc) else  (decode' tree l tl acc)  
            | Leaf (x, _) -> decode' tree tree bits (x::acc)
        | [] -> 
            match subtree with
            | Leaf (x, _) -> reverseList (x::acc)
            | _ -> failwith "Wrong bit sequence!"
    decode' tree tree bits []

let rec encode (tree: CodeTree)  (text: char list) : Bit list = 
    let charcode (tree: CodeTree) (char: char) : Bit list =
        let rec charencode' (tree: CodeTree) (char: char) (acc: Bit list) : Bit list =
            match tree with
            | Fork (l, r, check, _) when List.exists (fun x -> x = char) check ->
                match l with
                | Fork (_, _, clist, _) ->
                    if List.exists (fun x -> x = char) clist then
                        charencode' l char (0::acc)
                    else charencode' r char (1::acc)
                | Leaf (x, _) ->
                    if x = char then reverseList (0::acc) else reverseList (1::acc)
            | Leaf (_) -> reverseList acc
            | _ -> failwith "Unexpected symbol!" 
        charencode' tree char []
    match text with
    | hd::tl -> unite (charcode tree hd) (encode tree tl)
    | [] -> []

let tree = (Fork (Fork (Fork (Leaf('d', 1), Leaf ('c', 2), ['c';'d'], 3), Leaf ('b', 2), ['b';'c';'d'], 5), Leaf ('a', 3), ['a';'b';'c';'d'], 8)) in
    let blist = encode tree ['a';'a';'b';'c';'a';'b';'c';'d'] 
    let clist = decode tree blist
    printf "encode: %A\ndecode:%A" blist clist