﻿//----------------------------------------------------------------------------
//
// Copyright (c) 2014
//
//    Ryan Riley (@panesofglass) and Andrew Cherry (@kolektiv)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//----------------------------------------------------------------------------

[<AutoOpen>]
module Freya.Machine.Router.Syntax

open Freya.Router

(* Custom Operations

   Custom syntax operator extension used in the FreyaRouter computation
   expression. Custom syntax operators are used to register a resource,
   which is shorthand for registering a route with methods equal to All. *)

type FreyaRouterBuilder with

    (* Routes *)

    [<CustomOperation ("resource", MaintainsVariableSpaceUsingBind = true)>]
    member x.Resource (r, path, pipeline) =
        x.Route (r, All, path, pipeline)