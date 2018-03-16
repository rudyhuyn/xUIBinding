# CrossUIBinding
CrossUIBinding is a library allowing XAML/WinRT developers to use bindings (standard or native) on objects shared between windows.

The idea is very simple: the same object will be shared between all windows (in memory), but each window will virtually see a different instance of the object, a kind of ghost or wrapper (named <em>Clone</em>), so if a window launches a <em>PropertyChanged</em> event, it will not make the application crashes, instead, the library will individually propagate the <em>PropertyChanged</em> event to other windows on their own UI Thread!

# What do we try to solve?

Let's imagine an application with 2 windows (A and B), each window contains a <em>TextBlock</em> element with the <em>Text</em> property binded (using standard or native binding) to the same reference: <em>Artist.FullName</em>.

Now let's imagine that the window A allows the user to modify the value of FullName, once the value changed, the event <em>PropertyChanged</em> will be launched in order to update other bindings, whether it's the current window or the other windows.

In XAML/WinRT, <strong>each window has its own UI Thread</strong>, so when the A window launches <em>PropertyChanged</em>, it will launch this event on its own UI Thread and update its other bindings... but what about the window B?

B will also receive the event, but because this one was launched on the UI Thread of window A and not the one owned by window B, the application will crash, the same way an app crashes when you launch <em>PropertyChanged</em> on a non-UI thread.

<img class="alignnone size-large wp-image-6884" src="http://www.rudyhuyn.com/blog/wp-content/uploads/2018/03/Capture-1024x173.png" alt="" width="640" height="108" />

# Usage
In your ViewModel, first includes:
`using Huyn.CrossUIBinding;`

then modify the properties you want to share between windows to use:

<strong>before:</strong> `public double CrossUISliderValue { get; set; } = 1d;`

<strong>after: </strong> `public CrossUIItem&lt;double&gt; CrossUISliderValue { get; set; } = new CrossUIItem&lt;double&gt;(1d);`

or if you want the lib to launch automatically:

`public CrossUIItem&lt;double&gt; CrossUISliderValue { get; set; } = new CrossUIItem&lt;double&gt;(0d, true);`

Modify your bindings to include the (fake) Clone property:

<strong>before: </strong>
`{x:Bind ViewModel.CrossUISliderValue, Mode=TwoWay}`

<strong>after:</strong>
`{x:Bind ViewModel.CrossUISliderValue.Clone.Value, Mode=TwoWay}`

And use `CrossUISliderValue.Value` or `CrossUISliderValue.Clone.Value` when you want to change the value of the object.

If you set <em>autoRaisePropertyChanged</em> to false, you can manually raise <em>PropertyChanged </em>with `CrossUISliderValue.RaisePropertyChanged()`

# Limitation

There is only one known limitation with the current implementation: you can't update a property located at the right of the `Clone.Value` part of your bindings, if you do it, the application will crash the same way than with native bindings.

`{x:Bind ViewModel.Cities[1].Attractions.Zoo.Clone.Value.Animals.Count, Mode=TwoWay}`

In this example, you can launch PropertyChanged on ViewModel, Cities, Attractions or Zoo but if you do it on Animals or Count, your application will probably crash.

If you plan to raise PropertyChanged on Count for example, you should move your <em>CrossUIItem</em> the deepest possible in the object tree, in this case, on <em>Count</em>:

`{x:Bind ViewModel.Cities[1].Attractions.Zoo.Animals.Count.Clone.Value, Mode=TwoWay}`

# How to use the sample?
First download the sample app and click on <em>Create New Window</em> in order to create many other windows.

<img class="alignnone size-large wp-image-6924" src="http://www.rudyhuyn.com/blog/wp-content/uploads/2018/03/Capture-2-1010x1024.png" alt="" height="400" />

## Native Binding

```
public int Counter { get; set; } = 0;
<Run Text="{x:Bind ViewModel.Counter, Mode=OneWay}"/>
```

Click on the <em>increment</em> button in order to test the classic behavior. The application will crash, as expected.

## CrossUIBinding: property

```
public CrossUIBindingProvider<int> CounterCrossUI { get; set; } = new CrossUIBindingProvider<int>(0);
<Run Text="{x:Bind ViewModel.CounterCrossUI.Clone.Value, Mode=OneWay}"/>
```

Click on the <em>increment</em> button, the int will be incremented not only in the current window, but also in all other windows!

## CrossUIBinding: parent of the property

Same than previously, but this time the parent of the property is cross-UI compatible instead of the property.

## CrossUIBinding: UI Controls

Test each controls, when you update one of them, other windows are automatically update, without writing a single line of code.
