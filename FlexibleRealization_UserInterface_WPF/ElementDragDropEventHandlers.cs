namespace FlexibleRealization.UserInterface
{
    public delegate void ElementDragStarted_EventHandler(ElementBuilder dragged);

    public delegate void ElementDragCancelled_EventHandler(ElementBuilder dragged);

    public delegate void ElementDropCompleted_EventHandler(ElementBuilder dropped);
}
