(*
    A. Baygeldin (c) 2014
    Dictionary
*)

module MyMap
    
    open System
    open System.Collections
    open System.Collections.Generic
    
    (*Tree union*)

    type private Tree<'K, 'V when 'K:comparison and 'V:equality> =
        | Node of key: 'K * value: 'V * left: Tree<'K, 'V> * right: Tree<'K,'V> * height: int
        | Empty

    let private height t =
        match t with
        | Node(_, _, _, _, h) -> h
        | Empty -> 0

    let rec private maxNode t =
        match t with
        | Node(_, _, _, Empty, _) -> t
        | Node(_, _, _, right, _) -> maxNode right

    let rec private minNode t =
        match t with
        | Node(_, _, Empty, _, _) -> t
        | Node(_, _, left, _, _) -> minNode left

    let rec private existsKey k t =
        match t with
        | Node (k', _, left, right, _) ->
            if k > k' then existsKey k right elif k < k' then existsKey k left else true
        | Empty -> false
    
    let rec private count t =
        match t with
        | Node (_, _, left, right, _) -> count left + count right + 1
        | Empty -> 0

    let private isEmpty t = 
        match t with
        | Empty -> true
        | _ -> false

    let rec private getValueByKey k t =
        match t with
        | Node (k', v', left, right, _) ->
            if k > k' then getValueByKey k right elif k < k' then getValueByKey k left else v'
        | Empty -> failwith "Key not found!"

    let private makeNode k v l r = Node(k, v, l, r, max (height l) (height r) + 1)

    let private balance t = //4 types of rotation...
        match t with
        | Node(k, v, left, right, _) ->
            if height right - height left > 1 then
                match right with
                | Node(k', v', left', right', _) ->
                    if height left' >= height right'  then
                        match left' with
                        | Node(k'', v'', left'', right'', _) ->
                            let leftNode = makeNode k v left left''
                            let rightNode = makeNode k' v' right'' right'
                            makeNode k'' v'' leftNode rightNode
                        | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
                    else
                        match right' with
                        | Node(k'', v'', left'', right'', _) ->
                            let leftNode = makeNode k v left left'
                            let rightNode = makeNode k'' v'' left'' right''
                            makeNode k' v' leftNode rightNode
                        | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
                | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
            elif height left - height right > 1 then
                match left with
                | Node(k', v', left', right', _) ->
                    if height right' >= height left' then
                        match right' with
                        | Node(k'', v'', left'', right'', _) ->
                            let leftNode = makeNode k' v' left' left''
                            let rightNode = makeNode k v right'' right
                            makeNode k'' v'' leftNode rightNode
                        | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
                    else
                        match left' with
                        | Node(k'', v'', left'', right'', _) ->
                            let leftNode = makeNode k'' v'' left'' right''
                            let rightNode = makeNode k v right' right
                            makeNode k' v' leftNode rightNode
                        | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
                | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
            else t
        | Empty -> failwith "Can't balance tree. Possibly nodes contain wrong heights."
                

    let rec private add k v t = //non-tail...
        match t with
        | Node(k', _, _, _, _) when k = k' ->  t
        | Node(k', v', left, right, _) -> 
            let node = if k < k' then add k v left else add k v right
            if k < k' then
                Node(k', v', node, right, max (height node) (height right) + 1) 
            else
                Node(k', v', left, node, max (height left) (height node) + 1)
            |> balance  
        | Empty -> Node(k, v, Empty, Empty, 1)

    let rec private remove k t = //non-tail...
        match t with
        | Node(k', _, Empty, _, _) | Node(k', _, _, Empty, _) when k = k' -> Empty
        | Node(k', _, left, right, _) when k = k' -> 
            let leaf = if height left >= height right then maxNode left else minNode right
            match leaf with
            | Node(k'', v'', _, _, _) -> 
                let node = if height left >= height right then remove k'' left else remove k'' right
                if height left >= height right then
                    Node(k'', v'', node, right, max (height node) (height right) + 1)
                else
                    Node(k'', v'', left, node, max (height left) (height node) + 1)
        | Node(k', v', left, right, _) ->
            let node = if k < k' then remove k left else remove k right
            if k < k' then
                Node(k', v', node, right, max (height node) (height right) + 1) 
            else
                Node(k', v', left, node, max (height left) (height node) + 1)
            |> balance
        | Empty -> Empty
        

    (*Map Class*)

    type Map<'K, 'V when 'K:comparison and 'V:equality> =
        val private t : Tree<'K, 'V> 

        private new (src:Tree<'K, 'V>) =
            {
                t = src
            }

        new (src:seq<'K * 'V>) =
            {
                t = Seq.fold (fun acc (k, v) -> add k v acc) Empty src
            }

        member this.Add k v = new Map<_, _> (add k v this.t)
        member this.ContainsKey k = existsKey k this.t
        member this.Remove k = new Map<_, _> (remove k this.t)
        member this.Count = count this.t
        member this.IsEmpty = isEmpty this.t
        member this.Item k = getValueByKey k this.t
        member this.TryFind k = if existsKey k this.t then Some (getValueByKey k this.t) else None
        
        member private this.GetEnumerator() = 
            let rec toList t = 
                match t with
                | Node(k, v, left, right, _) -> (k, v) :: (toList left) @ (toList right)
                | Empty -> [] 
            let list =  toList this.t
            let current = ref list
            let head = ref true
            let getCurrent() = if !head then failwith("Can't take current") else (!current).Head    
            {
                new IEnumerator<'K * 'V> with
                    member this.Current = getCurrent()     
                interface IEnumerator with
                    member this.Current = getCurrent() :> obj
                    member this.MoveNext() = 
                        if !head then head := false
                        else current := (!current).Tail
                        not (!current).IsEmpty
                    member this.Reset() = 
                        head := true
                        current := list
                interface IDisposable with
                    member this.Dispose() = ()
            }

        
        interface IEnumerable<'K * 'V> with
            member this.GetEnumerator() = this.GetEnumerator()
        interface IEnumerable with
            member this.GetEnumerator() = this.GetEnumerator() :> IEnumerator
        
        override this.Equals x =
            match x with
            | :? Map<'K, 'V> as y -> (this.Count = y.Count) && (Seq.forall2 (=) this y)
            | _ -> false