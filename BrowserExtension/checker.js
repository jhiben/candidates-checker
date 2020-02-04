function checker(selector, nameCleaner) {
  if (!nameCleaner) {
    nameCleaner = t => t;
  }

  const _CHECKED_CLASS = 'candidates-checker-extension__checked';

  function setContacted(element, c) {
    const cod = new Date(c.contactedOn);
    const contactedOn = `${cod.getDate()}/${cod.getMonth() +
      1}/${cod.getFullYear()}`;

    element.classList.add(_CHECKED_CLASS);
    element.style.color = 'orange';
    element.title = `Probably contacted on ${contactedOn} by ${c.contactedBy}`;
  }

  let connected = undefined;

  function errorFetching(error) {
    if (connected === false) return;

    connected = false;
    chrome.runtime.sendMessage({ connectionLost: true });

    return Promise.reject(error);
  }

  function successFetching() {
    if (connected === true) return;

    connected = true;
    chrome.runtime.sendMessage({ connected: true });
  }

  function check(element) {
    if (!element.innerText || element.classList.contains(_CHECKED_CLASS))
      return;

    const text = nameCleaner(element.innerText);

    fetch('http://localhost:8466/api/check/' + encodeURIComponent(text))
      .catch(errorFetching)
      .then(r => r.json())
      .then(c => {
        successFetching();
        if (c.isContacted) {
          setContacted(element, c);
        }
      });
  }

  function searchTree(node) {
    node.querySelectorAll(selector).forEach(check);
  }

  var observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {
      for (let i = 0; i < mutation.addedNodes.length; i++) {
        const node = mutation.addedNodes[i];
        if (node.nodeType === Node.ELEMENT_NODE) {
          searchTree(node);
        }
      }
    });
  });

  observer.observe(document, { childList: true, subtree: true });

  searchTree(document);
}
