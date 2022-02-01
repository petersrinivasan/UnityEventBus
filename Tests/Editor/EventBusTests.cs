using NUnit.Framework;

namespace Petersrin.EventBus.Editor.Tests
{
  public class TestOneEvent : Event {
    public string Name { get; set; }
  }

  public class TestTwoEvent : Event {
    public string Name { get; set; }
  }

  [TestFixture]
  [Category("EventBus")]
  public class EventBusTests
  {

    private IEventBus _eventBus;
    
    private int _testOneEventCount;
    private string _testOneEventName = "";

    private int _testTwoEventCount;
    private string _testTwoEventName = "";

    private void OnTestOne(TestOneEvent e) {
      _testOneEventCount += 1;
      _testOneEventName = e.Name;
    }

    private void OnTestTwo(TestTwoEvent e) {
      _testTwoEventCount += 1;
      _testTwoEventName = e.Name;
    }

    [SetUp]
    public void Before()
    {
      _eventBus = new EventBus();
      _testOneEventCount = 0;
      _testOneEventName = "";
      _testTwoEventCount = 0;
      _testTwoEventName = "";
    }

    [Test]
    public void AddListenerIncrementsLookupCountOncePer() {
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 1);

      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 1);
    }

    [Test]
    public void RemoveListenerDecrementsLookupCountAlways() {
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      Assert.IsTrue(_eventBus.DelegateLookupCount ==  1);

      _eventBus.Unsubscribe<TestOneEvent>(OnTestOne);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 0);
    }

    [Test]
    public void RemoveListenerHandlesNoListeners() {
      Assert.IsTrue(_testOneEventCount == 0);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 0);

      _eventBus.Unsubscribe<TestOneEvent>(OnTestOne);
      Assert.IsTrue(_testOneEventCount == 0);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 0);
    }

    [Test]
    public void RaiseInvokes() {
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      _eventBus.Raise(new TestOneEvent() { Name="One A" });
      Assert.IsTrue(_testOneEventCount == 1);
      Assert.IsTrue(_testOneEventName == "One A");

      _eventBus.Raise(new TestOneEvent() { Name="One B" });
      Assert.IsTrue(_testOneEventCount == 2);
      Assert.IsTrue(_testOneEventName == "One B");
    }

    [Test]
    public void RaiseInvokesCorrectDelegate() {
      _eventBus.Subscribe<TestOneEvent>(OnTestOne);
      _eventBus.Subscribe<TestTwoEvent>(OnTestTwo);
      _eventBus.Raise(new TestTwoEvent() { Name="Two A" });
      Assert.IsTrue(_testTwoEventCount == 1);
      Assert.IsTrue(_testTwoEventName == "Two A");
      Assert.IsTrue(_testOneEventCount == 0);
      Assert.IsTrue(_testOneEventName == "");
    }

    [Test]
    public void RaiseHandlesNoListeners() {
      Assert.IsTrue(_testOneEventCount == 0);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 0);

      _eventBus.Raise(new TestOneEvent() { Name="One A" });
      Assert.IsTrue(_testOneEventCount == 0);
      Assert.IsTrue(_eventBus.DelegateLookupCount == 0);
    }
  }
}