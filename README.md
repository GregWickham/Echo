# Echo - A bridge from CoreNLP to SimpleNLG

Echo does two things:

1.  Transform output from the [Stanford CoreNLP](https://stanfordnlp.github.io/CoreNLP/) annotation pipeline into a specification that can be realized by [SimpleNLG](https://github.com/simplenlg/simplenlg); and
2.  Provide a user interface that can inspect the transformed specification as a directed graph.

Echo connects to both CoreNLP and SimpleNLG in their server forms.  This permits relatively loose coupling to both systems, and allows CoreNLP and SimpleNLG version updates without changing the .NET client code (unless the server API's change).

To use Echo, the first step is to install and run the servers for CoreNLP and SimpleNLG.  The folder called `Server BAT files` contains two Windows batch files that I use to start the servers.  These batch files refer to specific locations in my file system, so you'll need to modify them to reflect the actual locations where you install the distribution `.jar` files.  The SimpleNLG server requires two `.jar` files that are not included in the SimpleNLG distribution -- `hsqldb_6148.jar` and `lexAccess2013dist.jar`.

Once the servers are running, you're ready to run Echo.  You can test the SimpleNLG connection by using Visual Studio Test Explorer to run the unit tests in the `SimpleNLG_Tests` project.  The unit tests in `ParseAndRealize_Tests` exercise both the CoreNLP **and** SimpleNLG ends of the bridge.  Sometimes a few of the tests in `ParseAndRealize_Tests` will fail for reasons that I don't understand.  If this happens, try running those tests individually and they should work.

The Visual Studio startup project for the user interface is `Echo_UI_WPF`:

![Image of Echo WPF Window](/docs/images/EmptyWindow.jpg)

The menu bar contains drop-down items to set the hostname and port for the CoreNLP and SimpleNLG servers.  The default values are the same as the defaults distributed with each server -- *i.e.* `localhost:9000` for CoreNLP and `localhost:50007` for SimpleNLG.

To run the Echo pipeline, enter some text in the upper text box and press the "Parse" button:

![Image of parsed and realized sentence](/docs/images/ParsedAndRealizedRootSelected.jpg)

This causes the following to happen:

1.  The entered text is sent to the CoreNLP server and annotated.  
2.  Echo uses selected information from the [Part-Of-Speech](https://stanfordnlp.github.io/CoreNLP/pos.html), [Constituency](https://stanfordnlp.github.io/CoreNLP/parse.html), and [Enhanced++ Dependency](https://universaldependencies.org/u/dep/index.html) annotations to construct the object graph displayed in the window.
3.  The object graph builds an XML specification and sends it to the SimpleNLG server to be realized.
4.  The realized text is displayed in the text box at the bottom of the window.

In the graph, the nodes on the lowest layer represent parse tokens.  These nodes are different from the other nodes, and this difference is reflected in their visual style.  The other nodes represent the syntactic structure of the parsed text, and their labels correspond roughly to the specification for [Penn Treebank II Constituent Tags](http://www.surdeanu.info/mihai/teaching/ista555-fall13/readings/PennTreebankConstituents.html).  However this correspondence is not exact because certain transformations are necessary in order to generate a specification in the format required by SimpleNLG.

The edges of the graph represent parent / child syntactic relations.  The line styles and labels of each edge denote the child's syntactic role within its parent element.  Edges (syntactic roles) are labeled as follows:

* Unassigned = **?**
* Subject = **S**
* Predicate = **P**
* Head = **H**
* Modifier = **M**
* Complement = **C**
* Specifier = **S**
* Modal = **MD**
* Coordinator = **CD**
* Coordinated Element = **CE**
* Complementizer = **CM**
* Component = **W**

When the parse / realize process is complete, the root node of the graph is initially selected.  This selection is indicated in the above figure by a red border around the uppermost node.

On the right side of the Echo window is a tab control that displays detailed information about the node selected in the graph.  In the figure above, that selected node is the **Independent Clause** at the root of the graph.

We can select a different node in the graph by clicking on it:

![Image of selected coordinated prepositional phrase](/docs/images/ParsedAndRealizedCPPSelected.jpg)

Now the tab control displays properties for the selected **Coordinated Prepositional Phrase**; and the text box at the bottom displays the realized form of *only the selected element.*
