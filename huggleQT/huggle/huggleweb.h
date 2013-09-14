//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

#ifndef HUGGLEWEB_H
#define HUGGLEWEB_H

#include <QFrame>
#include "wikipage.h"
#include "core.h"

namespace Ui {
class HuggleWeb;
}

class HuggleWeb : public QFrame
{
    Q_OBJECT
    
public:
    explicit HuggleWeb(QWidget *parent = 0);
    ~HuggleWeb();
    void DisplayPreFormattedPage(WikiPage *page);
    void DisplayPreFormattedPage(QString url);
    void DisplayPage(QString url);
    void RenderHtml(QString html);
    
private:
    Ui::HuggleWeb *ui;
};

#endif // HUGGLEWEB_H
