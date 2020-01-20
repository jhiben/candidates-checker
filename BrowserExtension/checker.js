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
  if (!element.innerText || element.classList.contains(_CHECKED_CLASS)) return;

  fetch(
    'http://localhost:8466/api/check/' + encodeURIComponent(element.innerText),
  )
    .catch(errorFetching)
    .then(r => r.json())
    .then(c => {
      successFetching();
      if (c.isContacted) {
        setContacted(element, c);
      }
    });
}

const _PROFILE_MATCH = '.pv-top-card-v3--list > li.t-24';

function profileMatch(node) {
  return node.matches(_PROFILE_MATCH);
}

const _SEARCH_RESULT_MATCH = '.name.actor-name';

function searchResultsMatch(node) {
  return node.matches(_SEARCH_RESULT_MATCH);
}

const _SEARCH_POPUP_RESULT_MATCH =
  '.basic-typeahead__triggered-content.search-global-typeahead__content.search-box_focus .typeahead-suggestion .search-typeahead-v2__hit-info > span';

function searchPopupResultMatch(node) {
  return node.matches(_SEARCH_POPUP_RESULT_MATCH);
}

const _SEARCH_RECENT_MATCH =
  '.search-typeahead-v2__history-list-carousel > li .typeahead-suggestion--carousel-item-text';

function searchRecentMatch(node) {
  return node.matches(_SEARCH_RECENT_MATCH);
}

function match(node) {
  return (
    profileMatch(node) ||
    searchResultsMatch(node) ||
    searchPopupResultMatch(node) ||
    searchRecentMatch(node)
  );
}

var observer = new MutationObserver(mutations => {
  mutations.forEach(mutation => {
    for (let i = 0; i < mutation.addedNodes.length; i++) {
      const node = mutation.addedNodes[i];
      if (node.nodeType === Node.ELEMENT_NODE && match(node)) {
        check(node);
      }
    }
  });
});

observer.observe(document, { childList: true, subtree: true });

document
  .querySelectorAll(
    `${_PROFILE_MATCH},${_SEARCH_RESULT_MATCH},${_SEARCH_POPUP_RESULT_MATCH},${_SEARCH_RECENT_MATCH}`,
  )
  .forEach(check);
