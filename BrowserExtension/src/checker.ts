interface CheckResult {
  isContacted: boolean;
  contactedOn?: string;
  contactedBy?: string;
}

export function checker(
  selector: string,
  nameCleaner: (t: string) => string = t => t
) {
  const _CHECKED_CLASS = 'candidates-checker-extension__checked';

  function setContacted(element: HTMLElement, c: CheckResult) {
    let contactedOn = '?';
    if (c.contactedOn) {
      const cod = new Date(c.contactedOn);
      contactedOn = `${cod.getDate()}/${
        cod.getMonth() + 1
      }/${cod.getFullYear()}`;
    }

    element.classList.add(_CHECKED_CLASS);
    element.style.color = 'orange';
    element.title = `Probably contacted on ${contactedOn} by ${c.contactedBy}`;
  }

  let connected: boolean | undefined = undefined;

  function errorFetching() {
    if (connected === false) return;

    connected = false;
    chrome.runtime.sendMessage({ connectionLost: true });
  }

  function successFetching() {
    if (connected === true) return;

    connected = true;
    chrome.runtime.sendMessage({ connected: true });
  }

  function check(element: HTMLElement) {
    if (!element.innerText || element.classList.contains(_CHECKED_CLASS))
      return;

    const text = nameCleaner(element.innerText);

    fetch('http://localhost:8466/api/check/' + encodeURIComponent(text))
      .then(r => r.json())
      .then((c: CheckResult) => {
        successFetching();
        if (c.isContacted) {
          setContacted(element, c);
        }
      })
      .catch(errorFetching);
  }

  function searchTree(node: Document | Element) {
    node.querySelectorAll<HTMLElement>(selector).forEach(check);
  }

  var observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {
      for (let i = 0; i < mutation.addedNodes.length; i++) {
        const node = mutation.addedNodes[i];
        if (node.nodeType === Node.ELEMENT_NODE) {
          searchTree(node as Element);
        }
      }
    });
  });

  observer.observe(document, { childList: true, subtree: true });

  searchTree(document);
}
