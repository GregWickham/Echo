using System;

namespace FlexibleRealization.UserInterface
{
    public delegate void ElementDragStarted_EventHandler(Type draggedType);

    public delegate void ElementDragCancelled_EventHandler(Type draggedType);

    public delegate void ElementDropCompleted_EventHandler(Type droppedType);
}
